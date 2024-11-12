$(document).ready(function () {
    // Função para abrir o modal quando o botão "Beneficiários" é clicado
    $("button.btn-primary").click(function () {
        $("#beneficiariosModal").modal('show');
    });

    // Função para salvar beneficiário e adicionar na tabela
    $("#salvarBeneficiario").click(function () {
        var nome = $("#beneficiarioNome").val();
        var cpf = $("#beneficiarioCPF").val();

        // Verifica se ambos os campos estão preenchidos
        if (!nome || !cpf) {
            alert("Por favor, preencha todos os campos.");
            return;
        }

        // Envia os dados para o controller via AJAX
        $.ajax({
            url: '/Beneficiario/SalvarBeneficiario',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ nome: nome, cpf: cpf }),
            success: function (response) {
                if (response.sucesso) {
                    // Adiciona uma nova linha na tabela com os dados inseridos
                    var newRow = `
                        <tr data-id="${response.id}">
                            <td>${response.nome}</td>
                            <td>${response.cpf}</td>
                            <td>
                                <button type="button" class="btn btn-primary btn-sm alterarBeneficiario">Alterar</button>
                                <button type="button" class="btn btn-danger btn-sm excluirBeneficiario">Excluir</button>
                            </td>
                        </tr>
                    `;
                    $("#tabelaBeneficiarios tbody").append(newRow);

                    // Limpa o formulário e fecha o modal
                    $("#formBeneficiarios")[0].reset();
                    $("#beneficiariosModal").modal('hide');
                } else {
                    alert("Erro ao salvar beneficiário.");
                }
            },
            error: function () {
                alert("Erro ao salvar beneficiário.");
            }
        });
    });

    // Função para excluir beneficiário da tabela
    $(document).on('click', '.excluirBeneficiario', function () {
        var row = $(this).closest('tr');
        var id = row.data("id");

        // Envia o pedido de exclusão para o controller (pode adicionar uma chamada AJAX se necessário)
        $.ajax({
            url: '/Beneficiario/ExcluirBeneficiario',  // Atualize esta rota de acordo com a ação de exclusão do controller
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ id: id }),  // Passando o Id do beneficiário
            success: function (response) {
                if (response.sucesso) {
                    row.remove();  // Remove a linha da tabela
                    alert("Beneficiário excluído com sucesso.");
                } else {
                    alert("Erro ao excluir beneficiário.");
                }
            },
            error: function () {
                alert("Erro ao excluir beneficiário.");
            }
        });
    });
});
