import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCodeletComponent } from './create-codelet.component';

describe('CreateCodeletComponent', () => {
  let component: CreateCodeletComponent;
  let fixture: ComponentFixture<CreateCodeletComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateCodeletComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateCodeletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
