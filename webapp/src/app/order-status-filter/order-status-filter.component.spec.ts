import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderStatusFilterComponent } from './order-status-filter.component';

describe('OrderStatusFilterComponent', () => {
  let component: OrderStatusFilterComponent;
  let fixture: ComponentFixture<OrderStatusFilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrderStatusFilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderStatusFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
