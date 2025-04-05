# This simple PowerShell script will copy one or more Azure storage table from one location into another azure storage table
#
# Dependencies :
#	https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azcopy
#	https://docs.microsoft.com/en-us/powershell/azure/overview?view=azps-1.6.0
#
# Origin : 
#   https://gist.github.com/jtabuloc/0b80cd55630dd4bdf426681c6be7382b#file-azure-copy-azure-storage-table-ps1
#
# Usage :
#        Copy-AzureStorageTable -SrcStorageName "" -SrcAccessKey "" -DstStorageName "" -DstAccessKey "" -IncludeTable All  
#        Copy-AzureStorageTable -SrcStorageName "" -SrcAccessKey "" -DstStorageName "" -DstAccessKey "" -IncludeTable Table1,Table2,Table3  

function Copy-AzureStorageTable
{
    param
    (
        [parameter(Mandatory=$true)]
        [String]
        $SrcStorageName,

        [parameter(Mandatory=$true)]
        [String]
        $SrcAccessKey,

        [parameter(Mandatory=$true)]
        [String]
        $DstStorageName,

        [parameter(Mandatory=$true)]
        [String]
        $DstAccessKey,

        [parameter(Mandatory=$true)]
        [String[]]
        $IncludeTable
    )

    # Check if logged in
    Azure-Login

    # Source Account Storage Parameters
    $SrcContext = New-AzureStorageContext -StorageAccountName $SrcStorageName -StorageAccountKey $SrcAccessKey
    $SrcBaseUrl = "https://" + $SrcStorageName + ".table.core.windows.net/"

    # Destination Account Storage Parameters
    $DstContext = New-AzureStorageContext -StorageAccountName $DstStorageName -StorageAccountKey $DstAccessKey
    $DstTempContainer = "temptable"
    $DstBlobUrl = "https://" + $DstStorageName + ".blob.core.windows.net/$DstTempContainer"
    $DstTableUrl = "https://" + $DstStorageName + ".table.core.windows.net"

    # Create container in destination blob
    Write-Host "$DstTempContainer is not existing in $DstStorageName..."
    Write-Host "Creating container $DstTempContainer in $DstStorageName..."
    New-AzureStorageContainer -Name $DstTempContainer -Permission Off -Context $DstContext

    # Get all tables from source
    $SrcTables = Get-AzureStorageTable -Name "*" -Context $SrcContext

    foreach($table in $SrcTables)
    {
        $TableName = $table.Name                
        Write-Host "Table $TableName"

        # Validate if copy all table from source
        # Validate if table name is included in our list
        if(!$IncludeTable.Contains("All") -and !$IncludeTable.Contains($TableName))
        {
           Write-Host "Skipping table $TableName"
           continue
        }
                
        Write-Host "Migrating Table $TableName"      
        $SrcTableUrl = $SrcBaseUrl + $TableName

        # Copy Table from source to blob destination. As far as I know there is way no way to copy table to table directly.
        # Alternatively, we will copy the table temporaryly into destination blob.
        # Take note to put the actual path of AzCopy.exe
        Write-Host "Start exporting table $TableName..."
        Write-Host "From 	: $SrcTableUrl"
        Write-Host "To 		: $DstBlobUrl/$TableName"

        & "C:\Program Files (x86)\Microsoft SDKs\Azure\AzCopy\AzCopy.exe" /Source:$SrcTableUrl /Dest:$DstBlobUrl/$TableName /SourceKey:$SrcAccessKey /Destkey:$DstAccessKey
   
        # Get the newly created blob
        Write-Host "Get all blobs in $DstTempContainer..."
        $CurrentBlob = Get-AzureStorageBlob -Container $DstTempContainer -Prefix $TableName -Context $DstContext

        # Loop and check manifest, then import blob to table
        foreach($blob in $CurrentBlob)
        {
            if(!$blob.Name.contains('.manifest'))
            {
                continue
            }

            $manifest = $($blob.Name).split('/')[1]

            Write-Host "Start importing $TableName..."
            Write-Host "Source blob url : $DstBlobUrl/$TableName"
            Write-Host "Dest table url  : $DstTableUrl/$TableName"           
            Write-Host "Manifest name   : $manifest"
				
	        # Import blob to table. Insert entity if missing and update entity if exists
           & "C:\Program Files (x86)\Microsoft SDKs\Azure\AzCopy\AzCopy.exe" /Source:$DstBlobUrl/$TableName `
                              /Dest:$DstTableUrl/$TableName `
                              /SourceKey:$DstAccessKey `
                              /DestKey:$DstAccessKey `
                              /Manifest:$manifest `
                              /EntityOperation:"InsertOrReplace"            
        }
    }

    # Delete temp table storage after export and import process
    Write-Host "Removing $DstTempContainer from destination blob storage..."
    Remove-AzureStorageContainer -Name $DstTempContainer -Context $DstContext -Force
}

# Login
function Azure-Login
{
    $needLogin = $true

    Try 
    {
        $content = Get-AzureRmContext

        if ($content) 
        {
            $needLogin = ([string]::IsNullOrEmpty($content.Account))
        } 
    } 
    Catch 
    {
        if ($_ -like "*Login-AzureRmAccount to login*") 
        {
            $needLogin = $true
        } 
        else 
        {
            throw
        }
    }

    if ($needLogin)
    {
        Login-AzureRmAccount
    }
}