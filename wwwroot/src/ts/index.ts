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
        const navBar = document.querySelector('.navigation') as HTMLElement;
        const header = document.querySelector('header') as HTMLElement;
        const memberActions = header.querySelector('.member-actions') as HTMLElement;

        if (scrollOffset >= 20) {
            navBar.classList.add('fixed');
            header.style.borderBottom = 'none';
            memberActions.style.top = '26px';
        } else {
            navBar?.classList.remove('fixed');
            memberActions.style.top = '41px';
        }
    });

    //2:30 Central expressed as UTC
    const weddingDateTime = new Date('2023-07-03T19:30:00.000Z').getTime();
    const timeDivisors = {
        second: 1000,
        minute: 60000,
        hour: 3600000,
        day: 86400000,
        week: 604800000
    };

    const timeDivisorsArr = [timeDivisors.week, timeDivisors.day, timeDivisors.hour, timeDivisors.minute, timeDivisors.second];

    function getDifference() {
        var difference = weddingDateTime - Date.now();
        const parts = [] as number[];
        for (let divisor of timeDivisorsArr) {
            const val = Math.max(Math.floor(difference / divisor), 0);
            difference -= val * divisor;
            parts.push(val);
        }
        return parts;
    }

    function updateCountdown() {
        const [weeks, days, hours, minutes, seconds] = getDifference();
        
        //$('#countdown_weeks').text(weeks);
        $('#countdown_days').text(days + weeks * 7);
        $('#countdown_hours').text(hours);
        $('#countdown_minutes').text(minutes);
        $('#countdown_seconds').text(seconds);
    }
    setInterval(updateCountdown, 1000);

    $('.map-toggle-btn').click(e => {
        $(e.target.dataset.hide as string).hide();
        $(e.target.dataset.show as string).fadeIn();
    });

    function setupSlides() {
        function showSlide(index: number) {

            $($('.mySlides').removeClass('show').hide()[index]).addClass('show').show();
        }

        let index = 0;
        function nextSlide() {
            if (++index >= $('.mySlides').length) {
                index = 0;
            }
            showSlide(index);
        }
        function previousSlide() {
            if (--index < 0) {
                index = $('.mySlides').length - 1;
            }
            showSlide(index);
        }

        $('#prev-slide-btn').click(previousSlide);
        $('#next-slide-btn').click(nextSlide);

        setInterval(nextSlide, 10000);
        showSlide(index);
    }
    setupSlides();
    
});

