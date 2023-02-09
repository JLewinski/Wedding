import $ from 'jquery'

$(() => {
    const body = document.querySelector('body') as HTMLElement;
    $<HTMLAnchorElement>('a[href*="#"]:not([href="#"])').on('click', function (event) {
        if (this.hash !== "") {
            event.preventDefault();

            var $target = $(this.hash);
            $target = $target.length ? $target : $('[name=' + this.hash.slice(1) + ']');
            event.preventDefault();
            var top = $target?.offset()?.top ?? 0-90-body.scrollTop;
            console.log('top: ' + top);
            console.log('body: ' + body.scrollTop);
            top = top + body.scrollTop - 90;
            console.log('calulated: ' + top);
            $('html, body').animate({ scrollTop: top }, 800);
            return false;
        }
    });

    body.addEventListener('scroll', () => {
        const scrollOffset = window.pageYOffset || body.scrollTop;
        // console.log('window: ' + scrollOffset);
        // console.log('body: ' + body.scrollTop);
        const navBar = document.querySelector('.navigation') as HTMLElement;
        const header = document.querySelector('header') as HTMLElement;
        const memberActions = header.querySelector('.member-actions') as HTMLElement;
        // const navIcon = header.querySelector('.navicon') as HTMLElement;
    
        if (scrollOffset >= 20) {
            navBar.classList.add('fixed');
            header.style.borderBottom = 'none';
            // header.style.padding = '35px 0';
            memberActions.style.top = '26px';
            // navIcon.style.top = '34px';
        } else {
            navBar?.classList.remove('fixed');
            // header.style.borderBottom = 'solid 1px rgba(255, 255, 255, 0.2)';
            // header.style.padding = '50px 0';
            memberActions.style.top = '41px';
            // navIcon.style.top = '48px';
        }
    });
});

