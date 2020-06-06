import { Injectable } from '@angular/core';
import { Genre } from '../models/genre';

@Injectable({
  providedIn: 'root'
})
export class GenreService {

  genreList: Genre[] = [
    { id: '1', name: 'Rock' },
    { id: '2', name: 'Rap' },
    { id: '3', name: 'Samba' },
    { id: '4', name: 'Pagode' }
  ];

  constructor() { }

  getAllGenre(): Genre[] {
    return this.genreList;
  }
}
