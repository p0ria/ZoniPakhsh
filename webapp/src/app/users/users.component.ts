import { Component, OnInit } from '@angular/core';
import {UserService} from '../services/user.service';
import {Product} from '../models/Product';
import {NewProduct} from '../models/NewProduct';
import {User} from '../models/User';
import {NewUser} from '../models/NewUser';
import {Router} from '@angular/router';
import {ProductsResult} from '../models/ProductsResult';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  users: User[];
  selected: User;

  isModalOpen: boolean = false;
  isDeleteModalOpen: boolean = false;
  isEdit: boolean = false;
  newUser: NewUser = new NewUser();

  constructor(private userService: UserService,
              private router: Router) { }

  ngOnInit() {
    let sub = this.userService.getAllAdmins().subscribe(
      (admins: User[]) => {
        this.users = admins;
      },
      error => {
        console.log(error);
        if (error.status === 401)
          this.router.navigate(['/login']);
      }
    );
  }

  onNew(){
    this.isEdit = false;
    this.newUser = new NewUser();
    this.showModal();
  }

  onDelete(){
    this.showDeleteModal();
  }

  onEdit(){
    this.isEdit = true;
    this.newUser = new NewUser();
    this.newUser.Id = this.selected.Id;
    this.newUser.FirstName = this.selected.FirstName;
    this.newUser.LastName = this.selected.LastName;
    this.newUser.Phone = this.selected.Phone;
    this.newUser.Address = this.selected.Address;
    this.newUser.PostalCode = this.selected.PostalCode;
    this.showModal();
  }


  onOK(){
    if(this.isEdit) this.edit();
    else this.create();
    this.hideModal();
  }

  create(){
    let productSub = this.userService.addUser(this.newUser).subscribe(
      (user: User) => {
        this.users.push(user);
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
    let productSub = this.userService.editUser(this.newUser).subscribe(
      (user: User) => {
        let tobeUpdated = this.users.find(u => u.Id == user.Id);
        tobeUpdated.FirstName = user.FirstName;
        tobeUpdated.LastName = user.LastName;
        tobeUpdated.Phone = user.Phone;
        tobeUpdated.Address = user.Address;
        tobeUpdated.PostalCode = user.PostalCode;
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


  showModal(){
    this.isModalOpen = true;
  }

  hideModal(){
    this.isModalOpen = false;
  }


  onDeleteCancel(){
    this.hideDeleteModal();
  }

  onDeleteOK(){
    let deleteSub = this.userService.deleteUser(this.selected.Id).subscribe(
      (deleted: boolean) => {
        if(deleted){
          let index = this.users.findIndex(u => u.Id == this.selected.Id);
          if(index > -1) this.users.splice(index, 1);
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

}
