import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { mensagemModel } from '../../models/mensagem.model';
import { mensagemService } from '../../services/mensagem.service';

@Component({
  selector: 'app-contato',
  templateUrl: './contato.component.html'
})
export class ContatoComponent implements OnInit {

  mensagemGroup: FormGroup;

  constructor(private frm: FormBuilder, private service: mensagemService) { }

  ngOnInit() {
    this.mensagemGroup = this.frm.group({
      Nome: ['', [Validators.required, Validators.minLength(3)]],
      Email: ['', [Validators.required, Validators.email]],
      Telefone: ['', [Validators.required]],
      Mensagem: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(160)]]
    });
  }

  fnc_envia_mensagem() {
    const mensagem = new mensagemModel();

    mensagem.Nome = this.mensagemGroup.controls['Nome'].value;
    mensagem.Email = this.mensagemGroup.controls['Email'].value;
    mensagem.Telefone = this.mensagemGroup.controls['Telefone'].value;
    mensagem.Mensagem = this.mensagemGroup.controls['Mensagem'].value;

    this.service.fnc_envia_mensagem(mensagem).subscribe(x => console.log(x), (err) => console.log(err));
  }
}
