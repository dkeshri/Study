import { TestBed } from '@angular/core/testing';

import { HttpRequestHeaderInterceptor } from './http-request-header.interceptor';

describe('HttpRequestHeaderInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      HttpRequestHeaderInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: HttpRequestHeaderInterceptor = TestBed.inject(HttpRequestHeaderInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
