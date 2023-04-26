async function getArticleNamesData() {

    let artcleNamesResponse = await fetch(
        'https://localhost:7035/Article/GetArticlesNames');

    let data = await artcleNamesResponse.json();

    const ac = new Autocomplete(document.getElementById('article-search'), {
        data: data,
        maximumItems: 10,
        treshold: 3,
        onSelectItem: ({ label, value }) => {
            console.log("article selected:", label, value);
            location.replace(`/article/details/${value}`);

        }
    });
}

getArticleNamesData();