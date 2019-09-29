import { Injectable } from '@angular/core';

@Injectable()
export class ThemeService {
  themes = [
    { name: 'Light', href: '/styles/clr-ui.css'},
    { name: 'Dark', href: '/styles/clr-ui-dark.css' }
  ];

  linkRef: HTMLLinkElement;
  isDark: boolean;

  constructor() { }

  setTheme(theme) {
    localStorage.setItem('theme', JSON.stringify(theme));
    this.linkRef.href = theme.href;
    this.isDark = theme.name == 'Dark';
  }
}
