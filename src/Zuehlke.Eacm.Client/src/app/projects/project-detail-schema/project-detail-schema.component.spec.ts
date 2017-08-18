import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectDetailSchemaComponent } from './project-detail-schema.component';

describe('ProjectDetailSchemaComponent', () => {
  let component: ProjectDetailSchemaComponent;
  let fixture: ComponentFixture<ProjectDetailSchemaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectDetailSchemaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectDetailSchemaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
