import {Component, OnDestroy, OnInit} from '@angular/core';
import {CategoryService} from '../../services/category.service';
import {Category} from '../../models/Category';
import {Router} from '@angular/router';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss']
})
export class SideNavComponent implements OnInit {
  constructor(public categoryService: CategoryService) { }

  ngOnInit() {
    this.categoryService.getCategories();
  }
}
