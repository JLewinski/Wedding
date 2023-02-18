import $ from 'jquery'

$(() => {
    const body = document.querySelector('body') as HTMLElement;
    $<HTMLAnchorElement>('a[href*="#"]:not([href="#"])').on('click', function (event) {
        if (this.hash !== "") {
            event.preventDefault();

            var $target = $(this.hash);
            $target = $target.length ? $target : $('[name=' + this.hash.slice(1) + ']');
            event.preventDefault();
            var top = $target?.offset()?.top ?? 0 - 90 - body.scrollTop;
            top = top + body.scrollTop - 90;
            $('html, body').animate({ scrollTop: top }, 800);
            return false;
        } else {
            event.preventDefault();
            $('html, body').animate({ scrollTop: 0 }, 800);
        }
    });

    body.addEventListener('scroll', () => {
        const scrollOffset = window.pageYOffset || body.scrollTop;
        const navBar = document.querySelector('nav') as HTMLElement;

        if (scrollOffset >= 20) {
            navBar.classList.add('fixed');
        } else {
            navBar?.classList.remove('fixed');
        }
    });

    $('.map-toggle-btn').click(e => {
        $(e.target.dataset.hide as string).hide();
        $(e.target.dataset.show as string).fadeIn();
    });

    
    
});

