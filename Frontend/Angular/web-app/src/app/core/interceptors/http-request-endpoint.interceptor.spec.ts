import { TestBed } from '@angular/core/testing';

import { HttpRequestEndpointInterceptor } from './http-request-endpoint.interceptor';

describe('HttpRequestEndpointInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      HttpRequestEndpointInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: HttpRequestEndpointInterceptor = TestBed.inject(HttpRequestEndpointInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
