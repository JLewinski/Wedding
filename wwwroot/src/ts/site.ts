import { FormHelper } from "./helper";


export class TestCard {
    name: string;

    constructor(elementId: string = 'test') {
        this.name = 'Elisa Lewinski';
        console.log("It's working!!");
    }

    public build() {
        FormHelper.buildFormData(this);
    }
}

const testCard = new TestCard();
console.log(testCard);

window.addEventListener('scroll', () => {
    const scrollOffset = window.pageYOffset;
    const navBar = document.querySelector('.navigation') as HTMLElement;
    const header = document.querySelector('header') as HTMLElement;
    const memberActions = header.querySelector('.member-actions') as HTMLElement;
    const navIcon = header.querySelector('.navicon') as HTMLElement;

    if (scrollOffset >= 20) {
        navBar.classList.add('fixed');
        header.style.borderBottom = 'none';
        header.style.padding = '35px 0';
        memberActions.style.top = '26px';
        navIcon.style.top = '34px';
    } else {
        navBar?.classList.remove('fixed');
        header.style.borderBottom = 'solid 1px rgba(255, 255, 255, 0.2)';
        header.style.padding = '50px 0';
        memberActions.style.top = '41px';
        navIcon.style.top = '48px';
    }
})