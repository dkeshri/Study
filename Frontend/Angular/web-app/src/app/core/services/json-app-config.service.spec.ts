import { TestBed } from '@angular/core/testing';

import { JsonAppConfigService } from './json-app-config.service';

describe('JsonAppConfigService', () => {
  let service: JsonAppConfigService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JsonAppConfigService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
