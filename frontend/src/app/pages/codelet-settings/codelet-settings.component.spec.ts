import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CodeletSettingsComponent } from './codelet-settings.component';

describe('CodeletSettingsComponent', () => {
  let component: CodeletSettingsComponent;
  let fixture: ComponentFixture<CodeletSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CodeletSettingsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CodeletSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
