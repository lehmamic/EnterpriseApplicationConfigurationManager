import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectDetailOverviewComponent } from './project-detail-overview.component';

describe('ProjectDetailOverviewComponent', () => {
  let component: ProjectDetailOverviewComponent;
  let fixture: ComponentFixture<ProjectDetailOverviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectDetailOverviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectDetailOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
