import {Component, Inject, PLATFORM_ID} from '@angular/core';
import {LoginService} from './services/login.service';
import {DOCUMENT, isPlatformBrowser} from '@angular/common';
import {ThemeService} from './services/theme.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(public loginService: LoginService,
              @Inject(DOCUMENT) private document: Document,
              @Inject(PLATFORM_ID) private platformId: Object,
              public themeService: ThemeService) {
    if (isPlatformBrowser(this.platformId)) {
      let theme = this.themeService.themes[1];
      try {
        const stored = localStorage.getItem('theme');
        if (stored) {
          theme = JSON.parse(stored);
        }
      } catch (e) {
        // Nothing to do
      }
      let linkRef: HTMLLinkElement = this.document.createElement('link');
      linkRef.rel = 'stylesheet';
      this.document.querySelector('head').appendChild(linkRef);
      themeService.linkRef = linkRef;
      themeService.setTheme(theme);
    }
  }
}
