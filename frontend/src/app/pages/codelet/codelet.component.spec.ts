import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CodeletComponent } from './codelet.component';

describe('CodeletComponent', () => {
  let component: CodeletComponent;
  let fixture: ComponentFixture<CodeletComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CodeletComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CodeletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
