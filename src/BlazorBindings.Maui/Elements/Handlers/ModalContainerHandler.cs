﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Core;
using System;
using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements.Handlers
{
    public class ModalContainerHandler : IMauiContainerElementHandler, INonChildContainerElement
    {
        public ModalContainerHandler(NativeComponentRenderer renderer, DummyElement modalContainerDummyControl)
        {
            Renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            ModalContainerPlaceholderElementControl = modalContainerDummyControl ?? throw new ArgumentNullException(nameof(modalContainerDummyControl));

            _parentChildManager = new ParentChildManager<MC.NavigableElement, MC.Page>(ShowDialogIfPossible);
            _parentChildManager.ChildChanged += OnParentChildManagerChildChanged;
        }

        public NativeComponentRenderer Renderer { get; }
        public DummyElement ModalContainerPlaceholderElementControl { get; }
        public MC.Element ElementControl => ModalContainerPlaceholderElementControl;
        public object TargetElement => ElementControl;

        private readonly ParentChildManager<MC.NavigableElement, MC.Page> _parentChildManager;

        private void OnParentChildManagerChildChanged(object sender, EventArgs e)
        {
            if (_parentChildManager.Child != null)
            {
                _parentChildManager.Child.Disappearing += CleanUpDisappearingPage;
            }
        }

        private void CleanUpDisappearingPage(object sender, EventArgs e)
        {
            _parentChildManager.Child.Disappearing -= CleanUpDisappearingPage;
            _parentChildManager.Parent = null;
            _parentChildManager.Child = null;

            if (ClosedEventHandlerId != default)
            {
                Renderer.Dispatcher.InvokeAsync(() => Renderer.DispatchEventAsync(ClosedEventHandlerId, null, e));
            }
        }

        public ulong ClosedEventHandlerId { get; set; }

        public void ApplyAttribute(ulong attributeEventHandlerId, string attributeName, object attributeValue, string attributeEventUpdatesAttributeName)
        {
            switch (attributeName)
            {
                case "__ShowDialog":
                    if (attributeValue != null)
                    {
                        ShowDialogIfPossible(_parentChildManager);
                    }
                    break;
                case "onclosed":
                    Renderer.RegisterEvent(attributeEventHandlerId, id => { if (ClosedEventHandlerId == id) { ClosedEventHandlerId = 0; } });
                    ClosedEventHandlerId = attributeEventHandlerId;
                    break;
                default:
                    throw new NotImplementedException($"{GetType().FullName} doesn't recognize attribute '{attributeName}'");
            }
        }

        private void ShowDialogIfPossible(ParentChildManager<MC.NavigableElement, MC.Page> parentChildManager)
        {
            if (_parentChildManager.Parent != null && _parentChildManager.Child != null)
            {
                _parentChildManager.Parent.Navigation.PushModalAsync(_parentChildManager.Child);
            }
        }

        public void AddChild(MC.Element child, int physicalSiblingIndex)
        {
            _parentChildManager.SetChild(child);
        }

        public void RemoveChild(MC.Element child)
        {
            // TODO: This could probably be implemented at some point, but it isn't needed right now
        }

        public bool IsParented()
        {
            // Because this is a 'fake' element, all matters related to physical trees
            // should be no-ops.
            return false;
        }

        public void SetParent(MC.Element parent)
        {
            // This should never get called. Instead, INonChildContainerElement.SetParent() implemented
            // in this class should get called.
            throw new NotSupportedException();
        }

        public int GetChildIndex(MC.Element child)
        {
            // Because this is a 'fake' element, all matters related to physical trees
            // should be no-ops.
            return 0;
        }

        public void SetParent(object parentElement)
        {
            _parentChildManager.SetParent((MC.Element)parentElement);
        }

        public void Remove()
        {
            _parentChildManager.SetParent(null);
        }
    }
}
