import {Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {CategoryService} from '../services/category.service';
import {Category} from '../models/Category';
import {Subscription} from 'rxjs';
import {Router} from '@angular/router';
import {NewCategory} from '../models/NewCategory';
import {NewProduct} from '../models/NewProduct';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit{
  newCategory: NewCategory = new NewCategory();
  isModalOpen: boolean = false;
  isEdit: boolean = false;
  isDeleteModalOpen: boolean = false;
  selectedDelete: Category;

  @ViewChild('uploader') uploader: ElementRef;

  constructor(public categoryService: CategoryService) { }

  ngOnInit() {
    this.categoryService.getCategories();
  }

  onFileChange(event){
    let reader = new FileReader();
    if(event.target.files && event.target.files.length > 0){
      let file = event.target.files[0];
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.newCategory.ImageData = (<string>reader.result).split(',')[1];
        this.newCategory.ImageType = file.name.substr(file.name.lastIndexOf('.') + 1);
      };
    }else{
      this.newCategory.ImageData = null;
      this.newCategory.ImageType = null;
    }
  }

  onNew(){
    this.isEdit = false;
    this.newCategory = new NewCategory();
    this.showModal();
  }

  onEdit(selected: Category){
    this.isEdit = true;
    this.newCategory = new NewCategory();
    this.newCategory.Id = selected.Id;
    this.newCategory.Name = selected.Name;
    this.showModal();
  }

  onDelete(selected: Category){
    this.selectedDelete = selected;
    this.showDeleteModal();
  }

  showModal(){
    this.clearUploader();
    this.isModalOpen = true;
  }

  hideModal(){
    this.isModalOpen = false;
  }

  clearUploader(){
    this.uploader.nativeElement.value = "";
  }

  onCancel(){
    this.hideModal();
  }

  onOK(){
    if(this.isEdit) this.edit();
    else this.create();
    this.hideModal();
  }

  edit(){
    this.categoryService.editCategory(this.newCategory);
  }

  create(){
    this.categoryService.addCategory(this.newCategory);
  }

  onDeleteCancel(){
    this.hideDeleteModal();
  }

  onDeleteOK(){
    this.categoryService.deleteCategory(this.selectedDelete.Id);
    this.hideDeleteModal();
  }

  showDeleteModal(){
    this.isDeleteModalOpen = true;
  }

  hideDeleteModal(){
    this.isDeleteModalOpen = false;
  }



}
