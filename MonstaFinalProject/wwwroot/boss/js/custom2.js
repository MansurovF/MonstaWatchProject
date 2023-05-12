$(document).ready(function () {
    $(document).on('click', '.deleteBtn', function (e) {
        e.preventDefault();
        let url = $('.deleteBtn').attr('href');
        console.log(url)
        Swal.fire({
            title: 'Silmek istediyine eminsiniz?',
            text: "Bu prosesden geri qayida bilmeyessiniz!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Beli, Sil!'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(url)
                    .then(res => res.text())
                    .then(data => {
                        $('.indexContainer').html(data)
                    })
                Swal.fire(
                    'Deleted!',
                    'Your file has been deleted.',
                    'success'
                )
            }
        })
    })
})