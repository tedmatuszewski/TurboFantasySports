# Pull data out of entry list

URL: https://racerxonline.com/sx/2025/indianapolis/450sx/entry-list

```
let result = [];
document.querySelectorAll(".content_wrap .ui_table.zebra.mod_mobile tbody tr").forEach(row=>{
    result.push({
        number: parseInt(row.querySelector("td:nth-child(1)").innerText),
        name: row.querySelector("td:nth-child(2) a:nth-child(2)").innerText,
        id: row.querySelector("td:nth-child(2) a:nth-child(2)").href.replace('https://racerxonline.com/rider/', '').replace('/races', ''),
        class: 250
    });
});
console.log(JSON.stringify(result));
```

