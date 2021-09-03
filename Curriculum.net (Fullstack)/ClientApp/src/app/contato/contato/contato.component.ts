import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { mensagemModel } from '../../models/mensagem.model';
import { usuarioModel } from '../../models/usuario.model';
import { authService } from '../../services/auth.service';
import { mensagemService } from '../../services/mensagem.service';

@Component({
  selector: 'app-contato',
  templateUrl: './contato.component.html'
})
export class ContatoComponent implements OnInit {

  mensagemGroup: FormGroup;

  constructor(private frm: FormBuilder, private service: mensagemService, private authService: authService) {

  }

    ngOnInit() {
      this.mensagemGroup = this.frm.group({
        Nome: [this.authService.usrLogado.nome || '', [Validators.required, Validators.minLength(3)]],
        Email: [this.authService.usrLogado.email || '', [Validators.required, Validators.email]],
        Telefone: [this.authService.usrLogado.telefone || '', [Validators.required]],
      Mensagem: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(160)]]
    });
  }

  fnc_envia_mensagem() {
    const mensagem = new mensagemModel();

    mensagem.usr = new usuarioModel();

    mensagem.usr.nome = this.mensagemGroup.controls['Nome'].value;
    mensagem.usr.email = this.mensagemGroup.controls['Email'].value;
    mensagem.usr.telefone = this.mensagemGroup.controls['Telefone'].value;
    mensagem.Mensagem = this.mensagemGroup.controls['Mensagem'].value;

    this.service.fnc_envia_mensagem(mensagem).subscribe(x => console.log("Deu certo ?????"), (err) => console.log(err));
  }
}
