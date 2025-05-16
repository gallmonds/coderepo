import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecentCodeletsComponent } from './recent-codelets.component';

describe('RecentCodeletsComponent', () => {
  let component: RecentCodeletsComponent;
  let fixture: ComponentFixture<RecentCodeletsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecentCodeletsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecentCodeletsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
