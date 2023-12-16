import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RedirectToLandingPageComponent } from './redirect-to-landing-page.component';

describe('RedirectToLandingPageComponent', () => {
  let component: RedirectToLandingPageComponent;
  let fixture: ComponentFixture<RedirectToLandingPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RedirectToLandingPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RedirectToLandingPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
