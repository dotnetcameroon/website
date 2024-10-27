import postAsync from "./base/api";

// async function publishEvent(eventId) {
//     try {
//         const form = document.getElementById("event-form");
//         if (!form.checkValidity()) {
//             form.reportValidity();
//             return;
//         }

//         form.submit();
//         const response = await fetch(`/api/events/publish/${eventId}`, {
//             method: 'POST'
//         });

//         if (response.ok) {
//             window.location.href = '/admin';
//         } else {
//             window.location.href = '/errors';
//         }
//     } catch (error) {
//         console.error('An error occurred while publishing the event:', error);
//         window.location.href = '/errors';
//     }
// }

// async function cancelEvent(eventId) {
//     try {
//         const response = await fetch(`/api/events/cancel/${eventId}`, {
//             method: 'POST'
//         });

//         if (response.ok) {
//             window.location.href = '/admin';
//         } else {
//             window.location.href = '/errors';
//         }
//     } catch (error) {
//         console.error('An error occurred while cancelling the event:', error);
//         window.location.href = '/errors';
//     }
// }
