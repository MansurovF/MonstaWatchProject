$(document).ready(function (e) {


    $(document).on('click', '.rmvproduct', function (e) {
        e.preventDefault();
        let url = $(this).attr('href');
        fetch(url)
            .then(res => {
                return res.text()
            })
            .then(data => {
                $('.basket-block').html(data)
                fetch("/Basket/BasketDetails")
                    .then(res2 => {
                        return res2.text()
                    })
                    .then(data2 => {
                        $('.basketDetails').html(data2)
                    })
            })

    })

    $(document).on('click', '.removeWishlist', function (e) {
        e.preventDefault();
        let url = $(this).attr('href');
        fetch(url)
            .then(res => {
                return res.text()
            })
            .then(data => {
                $('.wishlist-block').html(data)
                fetch("/Wishlist/WishlistDetails")
                    console.log("asdasdasd")
                    .then(res2 => {
                        return res2.text()
                    })
                    .then(data2 => {
                        $('.wishlistDetails').html(data2)
                    })
            })
    })
    $(document).on('click', '.minus', function (e) {
        e.preventDefault();
        let url = $(this).attr('href');
        fetch(url)
            .then(res => {
                return res.text()
            })
            .then(data => {
                $('.basket-block').html(data)
                fetch("/Basket/BasketDetails")
                    .then(res2 => {
                        return res2.text()
                    })
                    .then(data2 => {
                        $('.basketDetails').html(data2)
                    })
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
    $(document).on("click", ' .addwish', function (e) {
        console.log("asdasds")
        e.preventDefault();

        let url = $(this).attr('href');
        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.cart-page').html(data)
                fetch('/Wishlist/GetWishlist')
                    .then(res1 => res1.text())
                    .then(data1 => {
                        $('.wishIconIndex').html(data1)

                    })
            })


    })

    $(document).on('click', '.plus', function (e) {
        e.preventDefault();
        let url = $(this).attr('href');
        fetch(url)
            .then(res => {
                return res.text()
            })
            .then(data => {
                $('.basket-block').html(data)
                fetch("/Basket/BasketDetails")
                    .then(res2 => {
                        return res2.text()
                    })
                    .then(data2 => {
                        $('.basketDetails').html(data2)
                    })
            })

    })

    
})