import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Webstatus } from './webstatus';

describe('Webstatus', () => {
  let component: Webstatus;
  let fixture: ComponentFixture<Webstatus>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Webstatus]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Webstatus);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
