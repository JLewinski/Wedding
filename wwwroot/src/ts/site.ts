function setupSlides() {
    const mySlides = document.querySelectorAll<HTMLElement>('.mySlides');
    function showSlide(index: number) {
        var i = 0;
        for (const slide of mySlides) {
            if (i++ == index) {
                slide.classList.add('show');
                slide.style.display = 'block';
            } else {
                slide.classList.remove('show');
                slide.style.display = 'none';
            }
        }
    }

    let index = 0;
    function nextSlide() {
        if (++index >= mySlides.length) {
            index = 0;
        }
        showSlide(index);
    }
    function previousSlide() {
        if (--index < 0) {
            index = mySlides.length - 1;
        }
        showSlide(index);
    }
    const interval = setInterval(nextSlide, 10000);

    document.getElementById('prev-slide-btn')?.addEventListener('click', () => { previousSlide(); clearInterval(interval); })
    document.getElementById('next-slide-btn')?.addEventListener('click', () => { nextSlide(); clearInterval(interval); });


    showSlide(index);


}

setupSlides();

function setupCountdown() {
    //2:30 Central expressed as UTC
    const weddingDateTime = new Date('2023-07-03T19:30:00.000Z').getTime();
    const timeDivisors = {
        second: 1000,
        minute: 60000,
        hour: 3600000,
        day: 86400000,
        week: 604800000
    };

    const timeDivisorsArr = [timeDivisors.day, timeDivisors.hour, timeDivisors.minute, timeDivisors.second];

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
        const nums = getDifference();

        //$('#countdown_weeks').text(weeks);
        const numDivs = document.querySelectorAll<HTMLElement>('.timer_number');
        for (const index in nums) {
            numDivs[index].textContent = nums[index].toFixed();
        }

    }
    setInterval(updateCountdown, 1000);
    updateCountdown();
}

setupCountdown();