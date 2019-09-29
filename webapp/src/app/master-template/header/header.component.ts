import {Component, Inject, OnInit, PLATFORM_ID} from '@angular/core';
import {LoginService} from '../../services/login.service';
import {ThemeService} from '../../services/theme.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  constructor(public loginService: LoginService,
              public themeService: ThemeService) {}

  ngOnInit() {
  }

  logout(){
    this.loginService.logout();
  }


}
