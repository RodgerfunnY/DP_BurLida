// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

(function () {
    const container = document.getElementById('notification-toast-container');
    if (!container) {
        return;
    }

    const pollUrl = container.getAttribute('data-notification-toast-url');
    const markReadUrl = container.getAttribute('data-notification-mark-read-url');
    const afToken = container.getAttribute('data-notification-af-token');
    if (!pollUrl) {
        return;
    }

    const pollIntervalMs = 18000;
    let pollTimer = null;
    /** id уведомлений, для которых toast уже показан, пока не закрыт (чтобы опрос не дублировал). */
    const pendingToastIds = new Set();

    function markAsRead(id) {
        if (!markReadUrl || !afToken || !id) {
            return;
        }
        const body = new URLSearchParams();
        body.append('id', String(id));
        body.append('__RequestVerificationToken', afToken);
        fetch(markReadUrl, {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body,
            credentials: 'same-origin'
        }).catch(function () { /* ignore */ });
    }

    function showToast(item) {
        if (typeof bootstrap === 'undefined' || !bootstrap.Toast) {
            return;
        }

        const wrap = document.createElement('div');
        wrap.className = 'toast';
        wrap.setAttribute('role', 'alert');
        wrap.setAttribute('aria-live', 'assertive');
        wrap.setAttribute('aria-atomic', 'true');

        const title = item.orderId
            ? 'Заявка №' + item.orderId
            : 'Уведомление';

        const detailsUrl = item.orderId ? '/Order/Details/' + item.orderId : null;
        const safeMessage = (item.message || '').replace(/</g, '&lt;').replace(/>/g, '&gt;');

        wrap.innerHTML =
            '<div class="toast-header bg-primary text-white">' +
            '<strong class="me-auto">' + title + '</strong>' +
            '<button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast" aria-label="Закрыть"></button>' +
            '</div>' +
            '<div class="toast-body small">' +
            safeMessage +
            (detailsUrl
                ? '<div class="mt-2"><a class="btn btn-sm btn-outline-primary" href="' + detailsUrl + '">Открыть</a></div>'
                : '') +
            '</div>';

        container.appendChild(wrap);

        const toast = new bootstrap.Toast(wrap, { autohide: true, delay: 10000 });
        wrap.addEventListener('hidden.bs.toast', function () {
            markAsRead(item.id);
            if (item.id != null) {
                pendingToastIds.delete(item.id);
            }
            wrap.remove();
        });
        toast.show();
    }

    async function pollUnread() {
        try {
            const response = await fetch(pollUrl, { credentials: 'same-origin' });
            if (!response.ok) {
                return;
            }
            const data = await response.json();
            const items = data.items || [];
            items.forEach(showToast);
        } catch (e) {
            /* сеть / сессия */
        }
    }

    function startPolling() {
        if (pollTimer) {
            clearInterval(pollTimer);
        }
        pollTimer = window.setInterval(pollUnread, pollIntervalMs);
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', function () {
            window.setTimeout(pollUnread, 2500);
            startPolling();
        });
    } else {
        window.setTimeout(pollUnread, 2500);
        startPolling();
    }
})();
