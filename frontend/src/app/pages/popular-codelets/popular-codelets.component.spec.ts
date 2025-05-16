import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PopularCodeletsComponent } from './popular-codelets.component';

describe('PopularCodeletsComponent', () => {
  let component: PopularCodeletsComponent;
  let fixture: ComponentFixture<PopularCodeletsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PopularCodeletsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PopularCodeletsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
