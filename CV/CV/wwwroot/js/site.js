setInterval(function () {
    fetch('/Chat/MessageBox', {
        method: 'GET'
    }).then(response => {
        // Handle the response here
        if (response.hasUnreadMessages) {
            document.getElementById('redDotId').style.display = 'block';
            setTimeout(function () {
                document.getElementById('redDotId').style.display = 'none';
            }, 60000);
        } else {
            document.getElementById('redDotId').style.display = 'none';
        }
    });
}, 10000);
