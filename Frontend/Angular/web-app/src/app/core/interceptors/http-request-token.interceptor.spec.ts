import { TestBed } from '@angular/core/testing';

import { HttpRequestTokenInterceptor } from './http-request-token.interceptor';

describe('HttpRequestInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      HttpRequestTokenInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: HttpRequestTokenInterceptor = TestBed.inject(HttpRequestTokenInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
