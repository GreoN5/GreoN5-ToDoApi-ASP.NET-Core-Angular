import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowCompleteDeleteComponent } from './show-complete-delete.component';

describe('ShowCompleteDeleteComponent', () => {
  let component: ShowCompleteDeleteComponent;
  let fixture: ComponentFixture<ShowCompleteDeleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowCompleteDeleteComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowCompleteDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
