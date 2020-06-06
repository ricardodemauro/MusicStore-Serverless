import { Component, OnInit } from '@angular/core';
import { GenreService } from '../services/genre.service';
import { Genre } from '../models/genre';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private genreService: GenreService) { }

  ngOnInit(): void {
  }

  listGenre(): Genre[] {
    return this.genreService.getAllGenre();
  }
}
