import { AbstractControl, FormGroup, ValidationErrors, ValidatorFn } from "@angular/forms";

export function senhaValidator(control: FormGroup): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    let Senha = control.get('Senha').value;
    let ConfirmacaoSenha = control.get('ConfirmacaoSenha').value;
    return Senha === ConfirmacaoSenha ? null : { naoCoincide: true }
  }
}
