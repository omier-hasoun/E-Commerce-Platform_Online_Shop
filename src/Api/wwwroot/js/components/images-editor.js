class ImagesEditor extends HTMLElement {
  constructor() {
    super();
    this._images = [];
    this.draggedIndex = null;
    this.ghostEl = null;
  }

  get multiple() { return this.hasAttribute('multiple'); }
  get accept() { return this.getAttribute('accept') || 'image/*'; }
  get maxSize() { return parseInt(this.getAttribute('max-size'), 10) || 10 * 1024 * 1024; }
  get maxImages() {
    const val = parseInt(this.getAttribute('max-images'), 10);
    return !isNaN(val) ? val : (this.multiple ? Infinity : 1);
  }

  // Setter for update mode (loading existing server images)
  set images(val) {
    this._images = (val || []).map(img => ({
      id: img.id || (Date.now().toString() + Math.random().toString(36).substr(2, 4)),
      url: img.url || '',
      rotation: img.rotation || 0,
      file: img.file || null
    }));
    if (this.isConnected) {
      if (this._images.length > this.maxImages) {
        this._images = this._images.slice(0, this.maxImages);
      }
      this.renderGrid();
      this.triggerChange();
    }
  }

  get images() {
    return this._images;
  }

  // Returns separated details and raw files list
  getData() {
    const files = [];
    const imagesDetails = this._images.map((img, index) => {
      let fileIndex = null;
      if (img.file) {
        files.push(img.file);
        fileIndex = files.length - 1;
      }
      return {
        sortOrder: index + 1,
        existingId: img.id,
        newFileIndex: fileIndex
      };
    });
    return { imagesDetails, files };
  }

  triggerChange() {
    this.dispatchEvent(new CustomEvent('change', {
      detail: this.getData(),
      bubbles: true,
      composed: true
    }));
  }

  connectedCallback() {
    if (this._images.length > this.maxImages) {
      this._images = this._images.slice(0, this.maxImages);
    }
    this.render();
    this.bindEvents();
    this.triggerChange();
  }

  render() {
    const maxMB = Math.round(this.maxSize / (1024 * 1024));
    const maxImgHint = this.maxImages !== Infinity ? ` | Max images: ${this.maxImages}` : '';

    this.innerHTML = `
      <div class="pim-dropzone" id="dropzone">
        <svg class="pim-dropzone-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"></path>
        </svg>
        <p>Drag & drop product images here, or click to browse</p>
        <span>Max size: ${maxMB}MB${maxImgHint}</span>
        <input type="file" id="file-input" class="pim-input-file" accept="${this.accept}" ${this.multiple ? 'multiple' : ''}>
      </div>

      <div class="pim-grid" id="image-grid"></div>
    `;

    this.renderGrid();
  }

  renderGrid() {
    const grid = this.querySelector('#image-grid');
    if (!grid) return;
    grid.innerHTML = '';

    this._images.forEach((img, index) => {
      const isCover = index === 0;
      const card = document.createElement('div');
      card.className = `pim-card ${isCover ? 'is-cover' : ''}`;
      card.dataset.index = index;

      card.innerHTML = `
        <div class="pim-img-wrap" data-drag-handle="true">
          <span class="pim-badge ${isCover ? 'cover-badge' : ''}">${isCover ? 'Cover' : '#' + (index + 1)}</span>
          <img src="${img.url}" loading="eager" class="pim-img" style="transform: rotate(${img.rotation}deg);" alt="Product Image">
        </div>
        <div class="pim-card-actions">
          ${!isCover && this.multiple ? `<button type="button" class="pim-cover-btn" data-action="set-cover">Set Cover</button>` : '<span></span>'}
          <div style="display: flex; gap: 0.25rem;">
            <button type="button" class="pim-action-btn" data-action="rotate" title="Rotate 90°">
              <svg width="14" height="14" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15"></path></svg>
            </button>
            <button type="button" class="pim-action-btn delete-btn" data-action="delete" title="Remove">
              <svg width="14" height="14" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"></path></svg>
            </button>
          </div>
        </div>
      `;

      card.querySelector('[data-action="rotate"]')?.addEventListener('click', () => this.rotateImage(index));
      card.querySelector('[data-action="delete"]')?.addEventListener('click', () => this.deleteImage(index));
      card.querySelector('[data-action="set-cover"]')?.addEventListener('click', () => this.setCover(index));

      if (this.multiple) {
        const handle = card.querySelector('[data-drag-handle="true"]');
        handle.addEventListener('pointerdown', (e) => this.onPointerDown(e, index, card));
      }

      grid.appendChild(card);
    });
  }

  bindEvents() {
    const dropzone = this.querySelector('#dropzone');
    const fileInput = this.querySelector('#file-input');

    dropzone.addEventListener('click', () => fileInput.click());
    dropzone.addEventListener('dragover', (e) => { e.preventDefault(); dropzone.classList.add('dragover'); });
    dropzone.addEventListener('dragleave', () => dropzone.classList.remove('dragover'));
    dropzone.addEventListener('drop', (e) => {
      e.preventDefault();
      dropzone.classList.remove('dragover');
      if (e.dataTransfer.files.length) this.handleFiles(e.dataTransfer.files);
    });

    fileInput.addEventListener('change', (e) => {
      if (e.target.files.length) this.handleFiles(e.target.files);
      fileInput.value = '';
    });
  }

  handleFiles(files) {
    const remainingSlots = this.maxImages - this._images.length;
    if (remainingSlots <= 0) {
      alert(`Maximum limit of ${this.maxImages} images reached.`);
      return;
    }

    const validFiles = Array.from(files).filter(file => {
      if (!file.type.startsWith('image/')) return false;
      if (file.size > this.maxSize) {
        alert(`File "${file.name}" exceeds the maximum size limit.`);
        return false;
      }
      return true;
    }).slice(0, remainingSlots);

    validFiles.forEach(file => {
      const reader = new FileReader();
      reader.onload = (e) => {
        this._images.push({
          id: null,
          url: e.target.result,
          rotation: 0,
          file: file
        });
        this.renderGrid();
        this.triggerChange();
      };
      reader.readAsDataURL(file);
    });
  }

  rotateImage(index) {
    this._images[index].rotation = (this._images[index].rotation + 90) % 360;
    this.renderGrid();
    this.triggerChange();
  }

  deleteImage(index) {
    this._images.splice(index, 1);
    this.renderGrid();
    this.triggerChange();
  }

  setCover(index) {
    if (!this.multiple) return;
    const [target] = this._images.splice(index, 1);
    this._images.unshift(target);
    this.renderGrid();
    this.triggerChange();
  }

  onPointerDown(e, index, card) {
    if (!this.multiple || (e.button !== 0 && e.pointerType === 'mouse')) return;

    this.draggedIndex = index;
    card.classList.add('is-dragging');

    const img = card.querySelector('img').src;
    this.ghostEl = document.createElement('div');
    this.ghostEl.className = 'pim-drag-ghost';
    this.ghostEl.innerHTML = `<img src="${img}">`;
    document.body.appendChild(this.ghostEl);

    this.moveGhost(e);

    const onPointerMove = (moveEvt) => {
      this.moveGhost(moveEvt);
      this.ghostEl.style.display = 'none';
      const targetEl = document.elementFromPoint(moveEvt.clientX, moveEvt.clientY);
      this.ghostEl.style.display = 'block';

      const targetCard = targetEl?.closest('.pim-card');
      if (targetCard && targetCard !== card) {
        const targetIndex = parseInt(targetCard.dataset.index, 10);
        if (!isNaN(targetIndex) && targetIndex !== this.draggedIndex) {
          const item = this._images.splice(this.draggedIndex, 1)[0];
          this._images.splice(targetIndex, 0, item);
          this.draggedIndex = targetIndex;
          this.renderGrid();
        }
      }
    };

    const onPointerUp = () => {
      window.removeEventListener('pointermove', onPointerMove);
      window.removeEventListener('pointerup', onPointerUp);
      if (this.ghostEl) {
        this.ghostEl.remove();
        this.ghostEl = null;
      }
      this.draggedIndex = null;
      this.renderGrid();
      this.triggerChange();
    };

    window.addEventListener('pointermove', onPointerMove);
    window.addEventListener('pointerup', onPointerUp);
  }

  moveGhost(e) {
    if (this.ghostEl) {
      this.ghostEl.style.left = `${e.clientX}px`;
      this.ghostEl.style.top = `${e.clientY}px`;
    }
  }
}

customElements.define('images-editor', ImagesEditor);
