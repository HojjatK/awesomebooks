import { TestBed, inject } from '@angular/core/testing';

import { CategoryAreaService } from './category-area.service';

describe('CategoryAreaService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CategoryAreaService]
    });
  });

  it('should be created', inject([CategoryAreaService], (service: CategoryAreaService) => {
    expect(service).toBeTruthy();
  }));
});
