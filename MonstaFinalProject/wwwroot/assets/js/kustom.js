$(document).ready(function () {


    $(document).on("click", '.rmvproduct', function (e) {
        e.preventDefault();

        let url = $(this).attr('href')

        fetch(url)
            .then(res => {
                return res.text();
            }).then(data => {
                $('.rmvproduct').html(data)
            })
    })
    $(document).on("click", '.pro-remove', function (e) {
        e.preventDefault();

        let url = $(this).attr('href')

        fetch(url)
            .then(res => {
                return res.text();
            }).then(data => {
                $('.pro-remove').html(data)
            })
    })

    $(document).on("click", ' .addbasket', function (e) {
        console.log("asdasds")
        e.preventDefault();

        let url = $(this).attr('href');
        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.cart-page').html(data)
                fetch('/Basket/GetBasket')
                    .then(res1 => res1.text())
                    .then(data1 => {
                        $('.basketIconIndex').html(data1)

                    })
            })


    })

    $(document).on("click", '  .rmvproduct, .plus', function (e) {
        e.stopPropagation();
        e.preventDefault();
        console.log(e.currentTarget)

        if (e.currentTarget.classList.contains('.rmvproduct')) {
            let url = $(e.currentTarget).attr('href');

            fetch(url)
                .then(res => {
                    return res.text();
                }).then(data => {
                    $('.header-cart').html(data)
                    console.log(url)
                    let url2 = "/" + url.split('/')[1] + "/sectionbasket"
                    console.log(url2)
                    conlole.log("salammm")
                    fetch(url2)
                        .then(res2 => {
                            return res2.text()
                        })
                        .then(data2 => {
                            $('.cart-page').html(data2)
                        })
                })

        } else if (e.currentTarget.classList.contains('plus') || e.currentTarget.classList.contains('plus')) {
            let url = $(e.currentTarget).attr('href');
            console.log(url);
            fetch(url)
                .then(res => {
                    return res.text();
                }).then(data => {
                    $('.header-cart').html(data)
                    console.log(url)
                    let url2 = "/" + url.split('/')[1] + "/refreshbasketsection"
                    console.log(url2)
                    
                    fetch(url2)
                        .then(res2 => {
                            return res2.text()
                        })
                        .then(data2 => {
                            $('.cart-page').html(data2)
                        })
                })
        }

    })

    $(document).on("click", '.minus', function (e) {
        console.log("starting")
        e.stopPropagation();
        e.preventDefault();
        console.log(e.currentTarget)
        fetchBasket();
        let url = $(this).attr('href');

        const fetchBasket = async () => {
            console.log("sdfsfad")
            fetch(url)
                .then(res => {
                    return await res.text();
            }).then(data => {
                $('.product-close').html(data)
                console.log("ur: l" + url)
                let url2 = "/" + url.split('/')[1] + "/sectionbasket"
                console.log("url2 " + url2)
                fetch(url2)
                    .then(res2 => {
                        return await res2.text()
                    })
                    .then(data2 => {
                        $('.cart-page').html(data2)
                    })
            })
        })
}

    $(document).on("click", '.rmvproduct', function (e) {
        e.preventDefault();
        console.log(e.currentTarget)

        let url = $(this).attr('href');

        fetch(url)
            .then(res => {
                return res.text();
            }).then(data => {
                $('.rmvproduct').html(data)
                console.log(url)
                let url2 = "/" + url.split('/')[1] + "/sectionbasket"
                console.log(url2)
                fetch(url2)
                    .then(res2 => {
                        return res2.text()
                    })
                    .then(data2 => {
                        $('.cart-page').html(data2)
                    })
            })


    })


})