import {Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {ProductService} from '../services/product.service';
import {ProductsResult} from '../models/ProductsResult';
import {ActivatedRoute, Router} from '@angular/router';
import {Product} from '../models/Product';
import {NewProduct} from '../models/NewProduct';
import {Subscription} from 'rxjs';
import {CategoryService} from '../services/category.service';


@Component({
  selector: 'app-product',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit, OnDestroy{
  private categoryId?: number;
  private sub: Subscription;
  private productsSub: Subscription;
  products: Product[];
  selected: Product;

  isModalOpen: boolean = false;
  isDeleteModalOpen: boolean = false;
  isEdit: boolean = false;
  newProduct: NewProduct = new NewProduct();

  @ViewChild('uploader') uploader: ElementRef;

  constructor(public productService: ProductService,
              public categoryService: CategoryService,
              public route: ActivatedRoute,
              public router: Router) { }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.categoryId = +params['id'];
      if(isNaN(this.categoryId)) this.categoryId = null;
      this.productsSub = this.productService.getCategoryProducts(this.categoryId).subscribe(
        (productsResult: ProductsResult) => {
          this.products = productsResult.Items;
        },
        error => {
          console.log(error);
          if (error.status === 401)
            this.router.navigate(['/login']);
        }
      )
    });
  }

  onNew(){
    this.isEdit = false;
    this.newProduct = new NewProduct();
    this.showModal();
  }

  onDelete(){
    this.showDeleteModal();
  }

  onEdit(){
    this.isEdit = true;
    this.newProduct = new NewProduct();
    this.newProduct.Id = this.selected.Id;
    this.newProduct.Name = this.selected.Name;
    this.newProduct.CategoryId = this.selected.Category.Id;
    this.newProduct.Price = this.selected.Price;
    this.newProduct.Count = this.selected.Count;
    this.showModal();
  }


  onOK(){
    if(this.isEdit) this.edit();
    else this.create();
    this.hideModal();
  }

  create(){
    let productSub = this.productService.addProduct(this.newProduct).subscribe(
      (product: Product) => {
        if(!this.categoryId || product.Category.Id == this.categoryId) this.products.push(product);
        productSub.unsubscribe();
      },
      error => {
        console.log(error);
        if (error.status === 401)
          this.router.navigate(['/login']);
        productSub.unsubscribe();
      }
    )
  }

  edit(){
    let productSub = this.productService.editProduct(this.newProduct).subscribe(
      (product: Product) => {
        let tobeUpdated = this.products.find(p => p.Id == product.Id);
        if(tobeUpdated) {
          if(!this.categoryId || this.categoryId == product.Category.Id){
            tobeUpdated.Name = product.Name;
            tobeUpdated.Price = product.Price;
            tobeUpdated.Count = product.Count;
            tobeUpdated.ImageUrl = product.ImageUrl;
            tobeUpdated.Category = product.Category;
          }else{
            let index = this.products.findIndex(p => p.Id == product.Id);
            this.products.splice(index, 1);
          }

        }
        productSub.unsubscribe();
      },
      error => {
        console.log(error);
        if (error.status === 401)
          this.router.navigate(['/login']);
        productSub.unsubscribe();
      }
    )
  }

  onCancel(){
    this.hideModal();
  }

  onFileChange(event){
    let reader = new FileReader();
    if(event.target.files && event.target.files.length > 0){
      let file = event.target.files[0];
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.newProduct.ImageData = (<string>reader.result).split(',')[1];
        this.newProduct.ImageType = file.name.substr(file.name.lastIndexOf('.') + 1);
      };
    }else{
      this.newProduct.ImageData = null;
      this.newProduct.ImageType = null;
    }
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

  onDeleteCancel(){
    this.hideDeleteModal();
  }

  onDeleteOK(){
    let deleteSub = this.productService.deleteProduct(this.selected.Id).subscribe(
      (deleted: boolean) => {
        if(deleted){
          let index = this.products.findIndex(p => p.Id == this.selected.Id);
          if(index > -1) this.products.splice(index, 1);
        }
        deleteSub.unsubscribe();
      },
      error => {
        console.log(error);
        if (error.status === 401)
          this.router.navigate(['/login']);
        deleteSub.unsubscribe();
      }
    );
    this.hideDeleteModal();
  }

  showDeleteModal(){
    this.isDeleteModalOpen = true;
  }

  hideDeleteModal(){
    this.isDeleteModalOpen = false;
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
    this.productsSub.unsubscribe();
  }

}
