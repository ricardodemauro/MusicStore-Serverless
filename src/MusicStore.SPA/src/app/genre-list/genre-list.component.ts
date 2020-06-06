import { Component, OnInit, Input } from '@angular/core';
import { Genre } from '../models/genre';

@Component({
  selector: 'app-genre-list',
  templateUrl: './genre-list.component.html',
  styleUrls: ['./genre-list.component.css']
})
export class GenreListComponent implements OnInit {
  @Input() genreListInput: Genre[];
  constructor() { }

  ngOnInit(): void {
    console.log('before logging');
    console.log(this.genreListInput);
  }
}
