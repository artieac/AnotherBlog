import React, { useEffect, useState } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { fetchListById, saveList, saveListItem, deleteListItem, setCurrentList } from '@/redux/BlogListSlice';
import { RootState, AppDispatch } from '@/redux/store';
import { TextInput } from '@/components/TextInput';
import { Button } from '@/components/Button';
import { Table } from '@/components/Table';
import { IBlogList, IBlogListItem } from '@/Models/IBlogList';

export const EditListPage: React.FC = () => {
    const { blogSubFolder, id } = useParams<{ blogSubFolder: string, id: string }>();
    const navigate = useNavigate();
    const dispatch = useDispatch<AppDispatch>();
    const { currentList, loading } = useSelector((state: RootState) => state.blogLists);

    const [name, setName] = useState('');
    const [showOrdered, setShowOrdered] = useState(false);

    const [editingItem, setEditingItem] = useState<IBlogListItem | null>(null);
    const [itemName, setItemName] = useState('');
    const [itemLink, setItemLink] = useState('');
    const [itemOrder, setItemOrder] = useState(0);

    useEffect(() => {
        if (blogSubFolder && id && id !== '-1') {
            dispatch(fetchListById({ blogSubFolder, id: parseInt(id) }));
        } else {
            dispatch(setCurrentList({ Id: 0, BlogId: 0, Name: '', ShowOrdered: false, Items: [] }));
        }
    }, [blogSubFolder, id, dispatch]);

    useEffect(() => {
        if (currentList) {
            setName(currentList.Name);
            setShowOrdered(currentList.ShowOrdered);
        }
    }, [currentList]);

    const handleSaveList = async (e: React.FormEvent) => {
        e.preventDefault();
        if (blogSubFolder) {
            const listToSave: IBlogList = {
                ...currentList!,
                Name: name,
                ShowOrdered: showOrdered,
            };
            const resultAction = await dispatch(saveList({ blogSubFolder, list: listToSave }));
            if (saveList.fulfilled.match(resultAction)) {
                if (id === '-1') {
                    navigate(`/Admin/App/EditList/${blogSubFolder}/${resultAction.payload.Id}`);
                }
            }
        }
    };

    const handleSaveItem = async (e: React.FormEvent) => {
        e.preventDefault();
        if (blogSubFolder && currentList && currentList.Id > 0) {
            const itemToSave: IBlogListItem = {
                Id: editingItem?.Id || 0,
                BlogListId: currentList.Id,
                Name: itemName,
                RelatedLink: itemLink,
                DisplayOrder: itemOrder,
            };
            await dispatch(saveListItem({ blogSubFolder, listId: currentList.Id, item: itemToSave }));
            setEditingItem(null);
            setItemName('');
            setItemLink('');
            setItemOrder(0);
        }
    };

    const handleEditItem = (item: IBlogListItem) => {
        setEditingItem(item);
        setItemName(item.Name);
        setItemLink(item.RelatedLink);
        setItemOrder(item.DisplayOrder);
    };

    const handleDeleteItem = async (itemId: number) => {
        if (blogSubFolder && currentList && window.confirm('Are you sure you want to delete this item?')) {
            await dispatch(deleteListItem({ blogSubFolder, listId: currentList.Id, itemId }));
        }
    };

    const itemColumns = [
        { header: 'Name', key: 'Name' },
        { header: 'Link', key: 'RelatedLink' },
        { header: 'Order', key: 'DisplayOrder' },
        { 
            header: 'Actions', 
            key: 'Actions', 
            render: (item: IBlogListItem) => (
                <div className="flex space-x-2">
                    <Button variant="secondary" size="sm" onClick={() => handleEditItem(item)}>Edit</Button>
                    <Button variant="danger" size="sm" onClick={() => handleDeleteItem(item.Id)}>Delete</Button>
                </div>
            ) 
        },
    ];

    if (loading && !currentList) return <div>Loading...</div>;

    return (
        <div className="space-y-6">
            <div className="bg-white shadow rounded-lg p-6">
                <div className="flex justify-between items-center mb-6">
                    <h1 className="text-2xl font-bold text-gray-900">
                        {id === '-1' ? 'Add List' : `Edit List: ${currentList?.Name}`}
                    </h1>
                    <Link to={`/Admin/App/ManageLists/${blogSubFolder}`}>
                        <Button variant="secondary">Back to Lists</Button>
                    </Link>
                </div>

                <form onSubmit={handleSaveList} className="space-y-4">
                    <TextInput
                        label="Name"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        required
                    />
                    <div className="flex items-center">
                        <input
                            type="checkbox"
                            id="showOrdered"
                            checked={showOrdered}
                            onChange={(e) => setShowOrdered(e.target.checked)}
                            className="h-4 w-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
                        />
                        <label htmlFor="showOrdered" className="ml-2 block text-sm text-gray-900">
                            Show Ordered
                        </label>
                    </div>
                    <Button type="submit" variant="primary">
                        {id === '-1' ? 'Create List' : 'Update List'}
                    </Button>
                </form>
            </div>

            {currentList && currentList.Id > 0 && (
                <div className="bg-white shadow rounded-lg p-6">
                    <h2 className="text-xl font-bold text-gray-900 mb-4">List Items</h2>
                    
                    <form onSubmit={handleSaveItem} className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-6 items-end">
                        <TextInput
                            label="Item Name"
                            value={itemName}
                            onChange={(e) => setItemName(e.target.value)}
                            required
                        />
                        <TextInput
                            label="Link"
                            value={itemLink}
                            onChange={(e) => setItemLink(e.target.value)}
                        />
                        <TextInput
                            label="Order"
                            type="number"
                            value={itemOrder.toString()}
                            onChange={(e) => setItemOrder(parseInt(e.target.value) || 0)}
                        />
                        <div className="flex space-x-2">
                            <Button type="submit" variant="primary">
                                {editingItem ? 'Update Item' : 'Add Item'}
                            </Button>
                            {editingItem && (
                                <Button type="button" variant="secondary" onClick={() => {
                                    setEditingItem(null);
                                    setItemName('');
                                    setItemLink('');
                                    setItemOrder(0);
                                }}>
                                    Cancel
                                </Button>
                            )}
                        </div>
                    </form>

                    <Table 
                        data={[...(currentList.Items || [])].sort((a, b) => a.DisplayOrder - b.DisplayOrder)} 
                        columns={itemColumns} 
                        keyField="Id" 
                    />
                </div>
            )}
        </div>
    );
};
