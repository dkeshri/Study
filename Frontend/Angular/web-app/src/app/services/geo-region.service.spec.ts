import { TestBed } from '@angular/core/testing';

import { GeoRegionService } from './geo-region.service';

describe('GeoRegionService', () => {
  let service: GeoRegionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GeoRegionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
