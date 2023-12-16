import { Injectable } from '@angular/core';
import { ApiService } from '../core/services/api/api.service';
import { Observable } from 'rxjs';
import { DocumentType } from './models/document.model';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {

  constructor(private apiService:ApiService) { }

  getDocumentTypes(): Observable<DocumentType[]> {
    return this.apiService.get<DocumentType[]>('DocumentTypes');
  }
}
