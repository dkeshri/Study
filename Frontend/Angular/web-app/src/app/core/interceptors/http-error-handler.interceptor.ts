import { MessageService } from 'primeng/api';
import { TokenService } from './../services/token.service';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
  HttpStatusCode
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { ApiError } from '../model/api-error';
import { Router } from '@angular/router';
import { AppConstants } from 'src/app/shared/models/app-constants';

@Injectable()
export class HttpErrorHandlerInterceptor implements HttpInterceptor {

  constructor(private tokenService: TokenService,
    private router: Router,
    private messageService: MessageService
  ) {

  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((err) => this.handleError(err))
    );
  }
  private handleError(error: HttpErrorResponse) {

    let httpResponseErrorObj: any;
    httpResponseErrorObj = error;
    switch (error.status) {
      case 0: this.messageService.add({ severity: 'error', summary: 'Client Error', detail: 'A client-side or network error occurred.', life: 3000 });
        break;
      case HttpStatusCode.UnprocessableEntity:
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Unprocessable entity', life: 3000 });
        break;
      case HttpStatusCode.NotFound:
        this.messageService.add({ severity: 'error', summary: 'Url Error', detail: 'Not Found', life: 3000 });
        break;
      case HttpStatusCode.Unauthorized:
        if(error.url?.includes('/auth/login')){
          this.messageService.add({ severity: 'error', summary: 'Unauthorized', detail: 'wrong username or password!', life: 3000 });
        }else{
          this.tokenService.logout();
          this.router.navigate([AppConstants.LOGIN_URL]);
          this.messageService.add({ severity: 'error', summary: 'Unauthorized', detail: 'Token Expire', life: 3000 });
        }

        break;
      case HttpStatusCode.InternalServerError:
        this.messageService.add({ severity: 'error', summary: 'Server Error', detail: 'internal server error', life: 3000 });
        break;
      case HttpStatusCode.BadRequest: let apiError = {} as ApiError;
        let errorBody = error.error;
        apiError.errorCode = errorBody.errorCode;
        apiError.errorData = errorBody.errorData;
        apiError.errorMessage = errorBody.errorMessage;
        httpResponseErrorObj = apiError;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: apiError.errorMessage, life: 3000 });
        break;
      default:
        console.log(error);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Something bad happened; please try again later.', life: 3000 })
        break;
    }
    return throwError(() => httpResponseErrorObj);
  }
}
