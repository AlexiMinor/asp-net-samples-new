document.getElementById('commments-toggle-btn')
    .addEventListener('click', collapseCommentsWithJsonData);


//async function collapseComments() {

//    let commentsViewResponse = await fetch(
//        'https://localhost:7035/Comment/GetFakeCommentsPartial');

//    let resp = await commentsViewResponse.text();


//    let commentBlock = document.getElementsByClassName('comments')[0];

//    commentBlock.innerHTML = resp;

//    const commentsCollapse = new bootstrap.Collapse('#collapsisbleComments', {
//        toggle: true
//    })
//}

async function collapseCommentsWithJsonData() {

    let commentsViewResponse = await fetch(
        'https://localhost:7035/Comment/GetFakeComments');

    let resp = await commentsViewResponse.json();
    let commentBlock = document.getElementsByClassName('comments')[0];
    for (let comment of resp) {
        let div = document.createElement("div");
        div.innerHTML = `<h5>${comment.user}</h5> <p>${comment.commentText}</p>`

        commentBlock.appendChild(div);
    }

    

    //

    //commentBlock.innerHTML = resp;

    const commentsCollapse = new bootstrap.Collapse('#collapsisbleComments', {
        toggle: true
    })
}