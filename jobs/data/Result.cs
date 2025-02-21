public class Result 
{
    public static int PointsFromPosition(int position)
    {
        switch (position)
        {
            case 1:
                return 25;
            case 2:
                return 22;
            case 3:
                return 20;
            case 4:
                return 18;
            case 5:
                return 17;
            case 6:
                return 16;
            case 7:
                return 15;
            case 8:
                return 14;
            case 9:
                return 13;
            case 10:
                return 12;
            case 11:
                return 11;
            case 12:
                return 10;
            case 13:
                return 9;
            case 14:
                return 8;
            case 15:
                return 7;
            case 16:
                return 6;
            case 17:
                return 5;
            case 18:
                return 4;
            case 19:
                return 3;
            case 20:
                return 2;
            case 21:
                return 1;
            default:
                return 0;
        }
    }
    
    public int Position { get; set; }

    public int Rider { get; set; }

    public int Points 
    { 
        get
        {
            switch (Position)
            {
                case 1:
                    return 25;
                case 2:
                    return 22;
                case 3:
                    return 20;
                case 4:
                    return 18;
                case 5:
                    return 17;
                case 6:
                    return 16;
                case 7:
                    return 15;
                case 8:
                    return 14;
                case 9:
                    return 13;
                case 10:
                    return 12;
                case 11:
                    return 11;
                case 12:
                    return 10;
                case 13:
                    return 9;
                case 14:
                    return 8;
                case 15:
                    return 7;
                case 16:
                    return 6;
                case 17:
                    return 5;
                case 18:
                    return 4;
                case 19:
                    return 3;
                case 20:
                    return 2;
                case 21:
                    return 1;
                default:
                    return 0;
            }
        } 
    }

    public string Class { get; set; }

    public string Race {get;set;}
}