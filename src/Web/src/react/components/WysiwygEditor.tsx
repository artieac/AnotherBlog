import React, { useLayoutEffect, useRef, useState } from 'react';
import ReactQuill, { Quill } from 'react-quill-new';
import { Delta } from 'quill';
import 'react-quill-new/dist/quill.snow.css';
import QuillTableBetter from 'quill-table-better';
import 'quill-table-better/dist/quill-table-better.css';

Quill.register({ 'modules/table-better': QuillTableBetter }, true);

// Quill's own bundled (but never enabled here) basic table module still
// auto-registers a clipboard matcher for <tr> as soon as `quill` is
// imported - `table: false` below only stops that module from being
// instantiated, it does not stop the matcher from registering. That
// matcher tags every table row with its own legacy `table` format on top
// of quill-table-better's formats, corrupting the delta produced when
// re-parsing saved post HTML back into the editor: the table renders fine
// on creation/preview (those never go through HTML->Delta conversion) but
// silently disappears the moment a saved post is loaded. Strip the legacy
// matcher from the clipboard quill-table-better registers, before it gets
// a chance to run.
const TableClipboard = Quill.import('modules/clipboard') as any;
class SafeTableClipboard extends TableClipboard {
    constructor(quill: any, options: any) {
        super(quill, options);
        this.matchers = this.matchers.filter(([selector]: [string, unknown]) => selector !== 'tr');
    }
}
Quill.register({ 'modules/clipboard': SafeTableClipboard }, true);

interface WysiwygEditorProps {
    value: string;
    onChange: (content: string) => void;
    label?: string;
    showPreview?: boolean;
}

export const WysiwygEditor: React.FC<WysiwygEditorProps> = ({ value, onChange, label, showPreview = false }) => {
    const [activeTab, setActiveTab] = useState<'edit' | 'preview'>('edit');
    const quillRef = useRef<ReactQuill>(null);
    const initialValueRef = useRef(value);

    // react-quill-new loads its initial `value` via Quill's `setContents`, which
    // (per quill-table-better's own README) does not build table blots correctly
    // and silently drops the table. Reload the initial content through
    // `updateContents` instead, the pattern quill-table-better documents for
    // initializing data. This only needs to run once per mount - the editor
    // itself must therefore never be unmounted/remounted (e.g. by the preview
    // tab toggle below), or this bug reappears on every remount.
    useLayoutEffect(() => {
        const initialValue = initialValueRef.current;
        const editor = quillRef.current?.getEditor();
        if (!editor || !initialValue || !initialValue.includes('<table')) {
            return;
        }
        const converted = editor.clipboard.convert({ html: initialValue });
        const delta = new Delta().delete(editor.getLength()).concat(converted);
        editor.updateContents(delta, Quill.sources.SILENT);
        editor.setSelection(0, Quill.sources.SILENT);
    }, []);

    const modules = {
        table: false,
        toolbar: [
            [{ 'header': [1, 2, 3, false] }],
            ['bold', 'italic', 'underline', 'strike', 'blockquote'],
            [{ 'list': 'ordered' }, { 'list': 'bullet' }, { 'indent': '-1' }, { 'indent': '+1' }],
            ['link', 'image'],
            ['table-better'],
            ['clean']
        ],
        'table-better': {
            language: 'en_US',
            menus: ['column', 'row', 'merge', 'table', 'cell', 'wrap', 'copy', 'delete'],
            toolbarTable: true
        },
        keyboard: {
            bindings: QuillTableBetter.keyboardBindings
        }
    };

    return (
        <div className="mb-4">
            <div className="flex justify-between items-center mb-1">
                {label && <label className="block text-sm font-medium text-gray-700">{label}</label>}
                {showPreview && (
                    <div className="flex border-b border-gray-200">
                        <button
                            type="button"
                            className={`py-1 px-4 text-sm font-medium ${activeTab === 'edit' ? 'text-blue-600 border-b-2 border-blue-600' : 'text-gray-500 hover:text-gray-700'}`}
                            onClick={() => setActiveTab('edit')}
                        >
                            Edit
                        </button>
                        <button
                            type="button"
                            className={`py-1 px-4 text-sm font-medium ${activeTab === 'preview' ? 'text-blue-600 border-b-2 border-blue-600' : 'text-gray-500 hover:text-gray-700'}`}
                            onClick={() => setActiveTab('preview')}
                        >
                            Preview
                        </button>
                    </div>
                )}
            </div>
            <div className="bg-white border rounded-md overflow-hidden">
                <div style={{ display: activeTab === 'edit' ? 'block' : 'none' }}>
                    <ReactQuill
                        ref={quillRef}
                        theme="snow"
                        value={value || ''}
                        onChange={onChange}
                        modules={modules}
                    />
                </div>
                {activeTab === 'preview' && (
                    <div className="ql-container ql-snow" style={{ border: 'none' }}>
                        <div
                            className="ql-editor p-4 min-h-[200px]"
                            dangerouslySetInnerHTML={{ __html: value || '' }}
                        />
                    </div>
                )}
            </div>
        </div>
    );
};
