import { Component, OnInit } from '@angular/core';
import { ConfigurationService } from 'src/app/core/services/configuration.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit {
  version!:string;
  constructor(private configurationService : ConfigurationService) { }

  ngOnInit(): void {
    this.version = this.configurationService.getVersion();
  }

}
