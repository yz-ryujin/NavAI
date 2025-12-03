
document.addEventListener("DOMContentLoaded", function () {

    const feedbackButtons = document.querySelectorAll(".btn-feedback");

    feedbackButtons.forEach(button => {
        button.addEventListener("click", function () {

            const container = this.closest(".feedback-container");

            const prompt = container.getAttribute("data-prompt");
            const response = container.getAttribute("data-response");
            const isUseful = this.getAttribute("data-util") === "true";

            const feedbackPergunta = container.querySelector(".feedback-pergunta");
            const feedbackAgradecimento = container.querySelector(".feedback-agradecimento");
            const todosBotoesDoContainer = container.querySelectorAll(".btn-feedback");

            sendFeedbackToBackend(prompt, response, isUseful);

            if (feedbackPergunta) {
                feedbackPergunta.style.display = "none";
            }

            if (feedbackAgradecimento) {
                feedbackAgradecimento.style.display = "inline";
            }
            todosBotoesDoContainer.forEach(btn => {
                btn.disabled = true;
                btn.classList.add("disabled");
            });
        });
    });

});

async function sendFeedbackToBackend(prompt, response, isUseful) {

    const feedbackData = {
        Prompt: prompt,
        Response: response,
        IsUseful: isUseful
    };

    try {

        const tokenInput = document.getElementsByName("__RequestVerificationToken")[0];

        if (!tokenInput) {
            console.error("Token de verificação (CSRF) não encontrado. O formulário está correto?");
            return;
        }

        const fetchResponse = await fetch("/Index?handler=Feedback", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": tokenInput.value
            },
            body: JSON.stringify(feedbackData)
        });

        if (fetchResponse.ok) {
            console.log("Feedback enviado com sucesso.");
        } else {
            console.error("Falha ao enviar feedback. Status: " + fetchResponse.status);
        }

    } catch (error) {
        console.error("Erro de rede ao enviar feedback:", error);
    }
}